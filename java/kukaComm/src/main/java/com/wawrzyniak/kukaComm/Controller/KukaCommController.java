package com.wawrzyniak.kukaComm.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.Records.ExceptionMessagePair;
import com.wawrzyniak.kukaComm.Model.SocketRequest.DataRequest;
import com.wawrzyniak.kukaComm.Model.Records.OutputWithErrors;
import com.wawrzyniak.kukaComm.Model.SocketRequest.SocketRequest;
import com.wawrzyniak.kukaComm.Model.SocketRequest.UnsubscribeRequest;
import com.wawrzyniak.kukaComm.Service.DataReading.KukaClientService;
import com.wawrzyniak.kukaComm.Service.DataSending.SessionManagerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

import java.io.IOException;
import java.util.HashMap;

public class KukaCommController extends TextWebSocketHandler {

    @Autowired
    private KukaClientService kukaService;
    @Autowired
    private SessionManagerService sessionService;
    private final ObjectMapper mapper = new ObjectMapper();

    @Override
    public void afterConnectionEstablished(WebSocketSession session) {
        sessionService.addSession(session);
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) {
        sessionService.removeSession(session);
    }

    @Override
    public void handleTextMessage(WebSocketSession session, TextMessage message) throws IOException {
        String request = message.getPayload();
        SocketRequest socketRequest = unpackRequest(session, request);
        if (socketRequest == null) return;
        switch (socketRequest.getRequestType()) {
            case UNSUBSCRIBE ->  handleDataRequest(session, (DataRequest) socketRequest);
            case DATA -> handleUnsubscribeRequest(session, (UnsubscribeRequest) socketRequest);
        }
    }

    private void handleDataRequest(WebSocketSession session, DataRequest data) throws IOException {
        try {
            kukaService.readVariable(data.getHost(), data.getVar());
            sessionService.addVariable(
                    session,
                    data.getHost(),
                    kukaService.getVariable(data.getHost(), data.getVar()));
        } catch (WrongIpException e) {
            try {
                session.sendMessage(new TextMessage(mapper.writeValueAsString(
                        new OutputWithErrors(
                                new HashMap<>(), new ExceptionMessagePair(
                                    e.getClass().getSimpleName(),
                                    e.getMessage(), 400)
                        ))));
            } catch (JsonProcessingException ex) {
                session.sendMessage(new TextMessage(e.getClass().getSimpleName() + e.getMessage()));
            }
        }
    }

    private void handleUnsubscribeRequest(WebSocketSession session, UnsubscribeRequest data){
        sessionService.removeRobot(session, data.getUnsubscribeIp());
    }

    private SocketRequest unpackRequest(WebSocketSession session, String request) throws IOException {
        SocketRequest data;
        try {
            data = mapper.readValue(request, SocketRequest.class);
        } catch (JsonProcessingException e) {
            try {
                session.sendMessage(new TextMessage(mapper.writeValueAsString(
                        new OutputWithErrors(
                                new HashMap<>(), new ExceptionMessagePair(
                                e.getClass().getSimpleName(), e.getMessage(), 500)
                        ))));
                return null;
            } catch (JsonProcessingException ex) {
                session.sendMessage(new TextMessage(ex.getClass().getSimpleName() + ex.getMessage()));
                return null;
            }
        }
        return data;
    }

    @Override
    public void handleTransportError(WebSocketSession session, Throwable exception) {
    }

    @Scheduled(fixedDelay = 30000)
    public void clearUnusedKukaThreads() {
        kukaService.removeDeadKukaThreads(sessionService.getObservedRobots());
    }
}
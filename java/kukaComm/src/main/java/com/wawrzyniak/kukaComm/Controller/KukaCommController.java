package com.wawrzyniak.kukaComm.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.Records.ExceptionMessagePair;
import com.wawrzyniak.kukaComm.Model.Records.IpVariablePair;
import com.wawrzyniak.kukaComm.Model.Records.OutputWithErrors;
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
        IpVariablePair data = getIpVariablePair(session, request);
        if (data == null) return;
        try {
            kukaService.readVariable(data.host(), data.var());
            sessionService.addVariable(
                    session,
                    data.host(),
                    kukaService.getVariable(data.host(), data.var()));
        } catch (WrongIpException e) {
            try {
                session.sendMessage(new TextMessage(mapper.writeValueAsString(
                        new OutputWithErrors(
                                new HashMap<>(), new ExceptionMessagePair(
                                        e.getClass().getSimpleName(), e.getMessage())
                        ))));
            } catch (JsonProcessingException ex) {
                session.sendMessage(new TextMessage(e.getClass().getSimpleName() + e.getMessage()));
            }
        }
    }

    private IpVariablePair getIpVariablePair(WebSocketSession session, String request) throws IOException {
        IpVariablePair data;
        try {
            data = mapper.readValue(request, IpVariablePair.class);
        } catch (JsonProcessingException e) {
            try {
                session.sendMessage(new TextMessage(mapper.writeValueAsString(
                        new OutputWithErrors(
                                new HashMap<>(), new ExceptionMessagePair(
                                e.getClass().getSimpleName(), e.getMessage())
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
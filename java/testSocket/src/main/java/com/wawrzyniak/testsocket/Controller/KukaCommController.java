package com.wawrzyniak.testsocket.Controller;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.testsocket.Exceptions.ExceptionTypes;
import com.wawrzyniak.testsocket.Model.Records.ExceptionMessagePair;
import com.wawrzyniak.testsocket.Model.Request.DataRequest;
import com.wawrzyniak.testsocket.Model.Records.OutputWithErrors;
import com.wawrzyniak.testsocket.Model.Request.SocketRequest;
import com.wawrzyniak.testsocket.Model.Request.UnsubscribeRequest;
import com.wawrzyniak.testsocket.Model.Types.VarType;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import com.wawrzyniak.testsocket.Service.SessionManagerService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

import java.util.HashMap;

public class KukaCommController extends TextWebSocketHandler {
    private static final Logger logger = LoggerFactory.getLogger(KukaCommController.class);

    @Autowired
    SessionManagerService sessionService;
    @Autowired
    KukaMockService kukaService;

    ObjectMapper mapper = new ObjectMapper();

    @Override
    public void afterConnectionEstablished(WebSocketSession session) {
        sessionService.addSession(session);

        logger.info("New session started: {}", session.getRemoteAddress().toString());
    }

    @Override
    public void handleTextMessage(WebSocketSession session, TextMessage message) throws Exception {
        String request = message.getPayload();
        SocketRequest socketRequest = mapper.readValue(request, SocketRequest.class);
        switch (socketRequest.getRequestType()){
            case DATA: {
                DataRequest data = (DataRequest) socketRequest;
                if (data.getVar() == VarType.WRONG) {
                    session.sendMessage(new TextMessage(mapper.writeValueAsString(
                            new OutputWithErrors(
                                    new HashMap<>(), new ExceptionMessagePair(
                                    ExceptionTypes.WRONG_IP.getException().getClass().getSimpleName(),
                                    ExceptionTypes.WRONG_IP.getException().getMessage(),
                                    400)
                            ))));
                    logger.info("Mocked exception sent to: {}", session.getRemoteAddress().toString());
                    return;
                }
                kukaService.addVariable(data.getHost(), data.getVar());
                sessionService.addVariable(session, data.getHost(), kukaService.getVariable(data.getHost(), data.getVar()));

                logger.info("Created connection to variable: {} ip: {}", data.getVar().name(), data.getHost());
                break;
            }
            case UNSUBSCRIBE:{
                UnsubscribeRequest data = (UnsubscribeRequest) socketRequest;
                logger.info("Unsubscribe request for ip: {}", data.getUnsubscribeIp());
                break;
            }
        }

    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) {
        sessionService.removeSession(session);

        logger.info("Session terminated: {} {}", session.getRemoteAddress().toString(), status.toString());
    }
}

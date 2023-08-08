package com.wawrzyniak.testsocket.Controller;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.testsocket.Model.Records.IpVariablePair;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import com.wawrzyniak.testsocket.Service.SessionManagerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

public class KukaCommController extends TextWebSocketHandler {

    @Autowired
    SessionManagerService sessionService;
    @Autowired
    KukaMockService kukaService;

    ObjectMapper mapper = new ObjectMapper();

    @Override
    public void afterConnectionEstablished(WebSocketSession session) {
        sessionService.addSession(session);
    }

    @Override
    public void handleTextMessage(WebSocketSession session, TextMessage message) throws Exception {
        String request = message.getPayload();
        IpVariablePair data = mapper.readValue(request, IpVariablePair.class);
        sessionService.addVariable(session, data.host(), kukaService.getVariable(data.var()));
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) {
        sessionService.removeSession(session);
    }
}

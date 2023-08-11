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

<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
import java.util.logging.Logger;

public class KukaCommController extends TextWebSocketHandler {

    private static final Logger logger = Logger.getLogger(KukaCommController.class.getName());

=======
public class KukaCommController extends TextWebSocketHandler {

>>>>>>> add testSocket and kukaComm
=======
import java.util.logging.Logger;

public class KukaCommController extends TextWebSocketHandler {

    private static final Logger logger = Logger.getLogger(KukaCommController.class.getName());

>>>>>>> add debug logging in websocket controller
    @Autowired
    SessionManagerService sessionService;
    @Autowired
    KukaMockService kukaService;

    ObjectMapper mapper = new ObjectMapper();

    @Override
    public void afterConnectionEstablished(WebSocketSession session) {
        sessionService.addSession(session);
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main

        logger.info("New session started: " + session.getRemoteAddress().toString());
=======
>>>>>>> add testSocket and kukaComm
=======

        logger.info("New session started: " + session.getRemoteAddress().toString());
>>>>>>> add debug logging in websocket controller
    }

    @Override
    public void handleTextMessage(WebSocketSession session, TextMessage message) throws Exception {
        String request = message.getPayload();
        IpVariablePair data = mapper.readValue(request, IpVariablePair.class);
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
        kukaService.addVariable(data.host(), data.var());
        sessionService.addVariable(session, data.host(), kukaService.getVariable(data.host(), data.var()));

        logger.info("Created connection to variable: " + data.var().name() + " ip: " + data.host());
=======
        sessionService.addVariable(session, data.host(), kukaService.getVariable(data.var()));
<<<<<<< refs/remotes/origin/main
>>>>>>> add testSocket and kukaComm
=======
=======
        kukaService.addVariable(data.host(), data.var());
        sessionService.addVariable(session, data.host(), kukaService.getVariable(data.host(), data.var()));
>>>>>>> Feature/testsocket setting data (#2)

        logger.info("Created connection to variable: " + data.var().name() + " ip: " + data.host());
>>>>>>> add debug logging in websocket controller
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) {
        sessionService.removeSession(session);
<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main

        logger.info("Session terminated: " + session.getRemoteAddress().toString() + " " + status.toString());
=======
>>>>>>> add testSocket and kukaComm
=======

        logger.info("Session terminated: " + session.getRemoteAddress().toString() + " " + status.toString());
>>>>>>> add debug logging in websocket controller
    }
}

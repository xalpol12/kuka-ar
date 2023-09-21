package com.wawrzyniak.testsocket.Service;

import com.wawrzyniak.testsocket.Model.KRLVar;
import org.springframework.stereotype.Service;
import org.springframework.web.socket.WebSocketSession;

import java.util.HashMap;
import java.util.Map;

@Service
public class SessionManagerService {
    private Map<WebSocketSession, ClientDataThread> threads;

    public SessionManagerService(){
        threads = new HashMap<>();
    }

    public void addSession(WebSocketSession session){
        threads.put(session, new ClientDataThread(session));
        threads.get(session).start();
    }

    public void removeSession(WebSocketSession session){
        threads.get(session).interrupt();
        threads.remove(session);
    }

    public void addVariable(WebSocketSession session, String hostIp, KRLVar var){
        threads.get(session).addVariable(hostIp, var);
    }

    public void removeRobot(WebSocketSession session, String host){
        threads.get(session).removeRobot(host);
    }

}

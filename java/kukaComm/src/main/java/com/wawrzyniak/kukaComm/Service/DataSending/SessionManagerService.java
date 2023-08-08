package com.wawrzyniak.kukaComm.Service.DataSending;

import com.wawrzyniak.kukaComm.Model.KRLVar;
import org.springframework.stereotype.Service;
import org.springframework.web.socket.WebSocketSession;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

@Service
public class SessionManagerService {
    private final Map<WebSocketSession, ClientDataThread> threads;

    public SessionManagerService(){
        threads = new HashMap<>();
    }

    public void addSession(WebSocketSession session) {
        threads.put(session, new ClientDataThread(session));
        threads.get(session).start();
    }

    public void removeSession(WebSocketSession session) {
        threads.get(session).interrupt();
        threads.remove(session);
    }

    public void addVariable(WebSocketSession session, String hostIp, KRLVar var) {
        threads.get(session).addVariable(hostIp, var);
    }

    public Set<String> getObservedRobots() {
       Set<String> robots = new HashSet<>();
       for (var entry : threads.entrySet()) {
           robots.addAll(entry.getValue().getObservedRobots());
       }
       return robots;
    }
}

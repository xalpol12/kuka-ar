package com.wawrzyniak.kukaComm.Service.DataSending;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.kukaComm.Model.KRLVar;
import com.wawrzyniak.kukaComm.Model.Records.ValueException;
import com.wawrzyniak.kukaComm.Model.Records.OutputWithErrors;
import lombok.SneakyThrows;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;

import java.util.*;

public class ClientDataThread extends Thread{

    private final WebSocketSession session;
    private final Map<String, List<KRLVar>> variables;
    private final ObjectMapper mapper;

    public ClientDataThread(WebSocketSession session) {
        this.session = session;
        variables = new HashMap<>();
        mapper = new ObjectMapper();
    }

    public Set<String> getObservedRobots(){
        return variables.keySet();
    }

    public void addVariable(String hostIp, KRLVar var) {
        if(!variables.containsKey(hostIp)) {
            variables.put(hostIp, new ArrayList<>());
        }
        if(variables.get(hostIp).contains(var)) {
            return;
        }
        if(var != null) {
            variables.get(hostIp).add(var);
        }
    }

    @SneakyThrows
    @Override
    public void run() {
        Map<String, Map<String, ValueException>> variablePacks = new HashMap<>();
        while(!Thread.currentThread().isInterrupted()) {
            variablePacks.clear();
            for (var entry : variables.entrySet()) {
                Map<String, ValueException> temp = new HashMap<>();
                for (KRLVar var : entry.getValue()) {
                    temp.put(var.getName(), new ValueException(
                            var.getValue(),
                            var.getReadExceptions()

                    ));
                }
                variablePacks.put(entry.getKey(), temp);
            }
            if(!variables.isEmpty()) {
                session.sendMessage(new TextMessage(mapper
                        .writeValueAsString(new OutputWithErrors(variablePacks, null))));
            }
            try {
                Thread.sleep(15);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }
    }
}

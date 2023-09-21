package com.wawrzyniak.testsocket.Service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.wawrzyniak.testsocket.Model.KRLVar;
import com.wawrzyniak.testsocket.Model.Records.ValueException;
import com.wawrzyniak.testsocket.Model.Records.OutputWithErrors;
import lombok.SneakyThrows;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;


import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ClientDataThread extends Thread {

    private Map<String, List<KRLVar>> variables;
    private WebSocketSession session;
    private ObjectMapper mapper;
    public ClientDataThread(WebSocketSession session){
        variables = new HashMap<>();
        this.session = session;
        mapper = new ObjectMapper();
    }
    public void addVariable(String hostIp, KRLVar var) {
        if(!variables.containsKey(hostIp)){
            variables.put(hostIp, new ArrayList<>());
        }
        if(variables.get(hostIp).contains(var)){
            return;
        }
        if(var != null) {
            variables.get(hostIp).add(var);
        }
    }

    public void removeRobot(String host) {
        if(variables.containsKey(host)){
            variables.remove(host);
        }
    }

    @Override
    @SneakyThrows
    public void run(){
        Map<String, Map<String, ValueException>> variablePacks = new HashMap<>();
        while(!Thread.currentThread().isInterrupted()) {
            variablePacks.clear();
            for (Map.Entry<String, List<KRLVar>> entry : variables.entrySet()) {
                Map<String, ValueException> temp = new HashMap<>();
                for (KRLVar var : entry.getValue()) {
                    temp.put(var.getName(), new ValueException(var.getValue(), var.getReadExceptions()));
                }
                variablePacks.put(entry.getKey(), temp);
            }
            if(!variables.isEmpty()){
                session.sendMessage(new TextMessage(mapper.writeValueAsString(new OutputWithErrors(variablePacks, null))));
            }
            try {
                Thread.sleep(15);
            } catch (InterruptedException e){
                Thread.currentThread().interrupt();
            }
        }
    }
}

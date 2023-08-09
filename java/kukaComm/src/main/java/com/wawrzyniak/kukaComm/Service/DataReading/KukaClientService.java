package com.wawrzyniak.kukaComm.Service.DataReading;

import com.wawrzyniak.kukaComm.Exceptions.WrongIpException;
import com.wawrzyniak.kukaComm.Model.KRLVar;
import com.wawrzyniak.kukaComm.Model.Types.VarType;
import org.springframework.stereotype.Service;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;


@Service
public class KukaClientService {
    private final Map<String, KukaClient> connections;
    private final Map<KukaClient, KukaClientThread> threads;

    KukaClientService() {
        connections = new HashMap<>();
        threads = new HashMap<>();
    }

    public void readVariable(String robotHost, VarType var) throws WrongIpException {
        if(!connections.containsKey(robotHost)) {
            try {
                connections.put(robotHost, new KukaClient(robotHost));
            } catch (IOException e) {
                connections.remove(robotHost);
                throw new WrongIpException("Cannot reach desired IP");
            }
        }
        KukaClient client = connections.get(robotHost);
        if(!threads.containsKey(client)) {
            threads.put(client, new KukaClientThread(client));
            threads.get(client).start();
        }
        client.addVariable(var);
        threads.get(client).addVariable(client.getVariable(var));
    }

    public void removeDeadKukaThreads(Set<String> aliveIp) {
        Set<String> all = connections.keySet();
        for(String ip : all) {
            if(!aliveIp.contains(ip)) {
                closeThread(ip);
            }
        }
    }

    private void closeThread(String ip) {
        threads.get(connections.get(ip)).interrupt();
        threads.remove(connections.get(ip));
        connections.remove(ip);
    }

<<<<<<< refs/remotes/origin/main
<<<<<<< refs/remotes/origin/main
    public KRLVar getVariable(String hostIp, VarType var) {
=======
    public KRLVar getVariable(String hostIp, VarType var){
>>>>>>> add testSocket and kukaComm
=======
    public KRLVar getVariable(String hostIp, VarType var) {
>>>>>>> add swagger docs, fix some whitespace issues
        return connections.get(hostIp).getVariable(var);
    }

}

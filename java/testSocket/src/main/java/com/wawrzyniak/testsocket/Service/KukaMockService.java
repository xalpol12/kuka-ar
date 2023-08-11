package com.wawrzyniak.testsocket.Service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Model.KRLVar;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Types.RobotClasses;
import com.wawrzyniak.testsocket.Model.Types.VarType;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import lombok.Getter;
import lombok.Setter;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.logging.Logger;

@Service
public class KukaMockService {

    private final static Logger logger = Logger.getLogger(KukaMockService.class.getName());

    private Map<String, Map<VarType, KRLVar>> variables;
    @Setter
    @Getter
    private boolean randomizing;

    public KukaMockService() {
        variables = new HashMap<>();
        randomizing = false;
        randomize();
    }

    public Map<String, Map<String, RobotData>> getAvailableRobots() {
        MockRobotListBuilder listBuilder = new MockRobotListBuilder();
        for (RobotClasses robot : RobotClasses.values()){
            Set<RobotData> robots = new HashSet<>();
            for (int i = 0; i < 3; i++){
                robots.add(new RobotDataBuilder()
                        .withName(robot.name() + "_" + i)
                        .withPositionShift(i, i * 1.5, i * 2)
                        .withRotationShift(i, i*1.5, i*2)
                        .build());
            }
            listBuilder.withRobots(robot.name(), robots);
        }
        return listBuilder.build();
    }

    @Scheduled(fixedDelay = 10000)
    private void scheduledRandomization() {
        if(randomizing) {
            randomize();
        }
    }

    public void randomize() {
        Set<Map.Entry<String, Map<VarType, KRLVar>>> entries = variables.entrySet();
        for(Map.Entry<String, Map<VarType, KRLVar>> vars : entries) {
            for(KRLVar var : vars.getValue().values()){
                var.setRandomValues();
            }
        }
    }

    public void addVariable(String hostIp, VarType var) {
        if(!variables.containsKey(hostIp)) {
            variables.put(hostIp, new HashMap<>());
        }
        if(!variables.get(hostIp).containsKey(var)){
            variables.get(hostIp).put(var, new KRLVar(var));
            variables.get(hostIp).get(var).setRandomValues();
        }
    }

    public KRLVar getVariable(String host, VarType var) {
        return variables.get(host).get(var);
    }

    public KRLValue setValue(String host, VarType var, KRLValue value) throws JsonProcessingException {
        addVariable(host, var);
        logger.info("Changed variable value " + host +
                " " + var + " to " + value.toJsonString());
        return variables.get(host).get(var).setValue(value);
    }
}

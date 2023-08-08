package com.wawrzyniak.testsocket.Service;

<<<<<<< refs/remotes/origin/main
import com.fasterxml.jackson.core.JsonProcessingException;
=======
>>>>>>> add testSocket and kukaComm
import com.wawrzyniak.testsocket.Model.KRLVar;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Types.RobotClasses;
import com.wawrzyniak.testsocket.Model.Types.VarType;
<<<<<<< refs/remotes/origin/main
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import lombok.Getter;
import lombok.Setter;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.logging.Logger;
=======
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.util.HashSet;
import java.util.Map;
import java.util.Set;
>>>>>>> add testSocket and kukaComm

@Service
public class KukaMockService {

<<<<<<< refs/remotes/origin/main
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
=======
    private KRLVar base;
    private KRLVar baseNumber;
    private KRLVar position;
    private KRLVar joints;
    private KRLVar toolNumber;

    public KukaMockService(){
        base = new KRLVar(VarType.BASE);
        baseNumber = new KRLVar(VarType.BASE_NUMBER);
        position = new KRLVar(VarType.POSITION);
        joints = new KRLVar(VarType.JOINTS);
        toolNumber = new KRLVar(VarType.TOOL_NUMBER);
        randomize();
    }

    public Map<String, Map<String, RobotData>> getAvailableRobots(){
>>>>>>> add testSocket and kukaComm
        MockRobotListBuilder listBuilder = new MockRobotListBuilder();
        for (RobotClasses robot : RobotClasses.values()){
            Set<RobotData> robots = new HashSet<>();
            for (int i = 0; i < 3; i++){
                robots.add(new RobotDataBuilder()
<<<<<<< refs/remotes/origin/main
                        .withName(robot.name() + "_" + i)
=======
                        .withName(robot.name() + "_" + String.valueOf(i))
>>>>>>> add testSocket and kukaComm
                        .withPositionShift(i, i * 1.5, i * 2)
                        .withRotationShift(i, i*1.5, i*2)
                        .build());
            }
            listBuilder.withRobots(robot.name(), robots);
        }
        return listBuilder.build();
    }

    @Scheduled(fixedDelay = 10000)
<<<<<<< refs/remotes/origin/main
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
=======
    private void randomize(){
        base.setRandomValues();
        baseNumber.setRandomValues();
        position.setRandomValues();
        joints.setRandomValues();
        toolNumber.setRandomValues();
    }

    public KRLVar getVariable(VarType var){
        switch (var){
            case BASE -> {
                return base;
            }
            case POSITION -> {
                return position;
            }
            case BASE_NUMBER -> {
                return baseNumber;
            }
            case JOINTS -> {
                return joints;
            }
            case TOOL_NUMBER -> {
                return toolNumber;
            }
        }
        return null;
>>>>>>> add testSocket and kukaComm
    }
}

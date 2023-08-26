package com.wawrzyniak.testsocket.Service;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Exceptions.ExceptionTypes;
import com.wawrzyniak.testsocket.Exceptions.WrongRequestException;
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

    public void randomize() {
        Set<Map.Entry<String, Map<VarType, KRLVar>>> entries = variables.entrySet();
        for (Map.Entry<String, Map<VarType, KRLVar>> vars : entries) {
            for(KRLVar var : vars.getValue().values()){
                var.setRandomValues();
            }
        }
    }

    public void addVariable(String hostIp, VarType var) {
        if (!variables.containsKey(hostIp)) {
            variables.put(hostIp, new HashMap<>());
        }
        if (!variables.get(hostIp).containsKey(var)){
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

    public void addExceptionToVariable(String hostIp, VarType variable, ExceptionTypes exception) throws WrongRequestException {
        if (exception == ExceptionTypes.WRONG_IP || exception == ExceptionTypes.DISCONNECTED) {
            throw new WrongRequestException("This exception cannot be added to variable");
        }
        if (!variables.containsKey(hostIp)) {
            throw new WrongRequestException("Robot with chosen ip is not connected to websocket at the moment. Try to reach it through websocket first.");
        }
        if (!variables.get(hostIp).containsKey(variable)) {
            throw new WrongRequestException("Requested variable does not exists. It would be appropriate to firstly request reading fo chosen variable through websocket.");
        }
        variables.get(hostIp).get(variable).addReadException(exception.getException());
    }

    public void removeExceptionFromVariable(String hostIp, VarType variable) throws WrongRequestException {
        if (!variables.containsKey(hostIp)) {
            throw new WrongRequestException("Robot with chosen ip is not connected to websocket at the moment. Try to reach it through websocket first.");
        }
        if (!variables.get(hostIp).containsKey(variable)) {
            throw new WrongRequestException("Requested variable does not exists. It would be appropriate to firstly request reading fo chosen variable through websocket.");
        }
        variables.get(hostIp).get(variable).clearExceptions();
    }

    public void disconnectRobot(String hostIp) throws WrongRequestException {
        if (!variables.containsKey(hostIp)) {
            throw new WrongRequestException("Robot with chosen ip is not connected to websocket at the moment. Try to reach it through websocket first.");
        }
        for (Map.Entry<VarType, KRLVar> entry : variables.get(hostIp).entrySet()) {
            entry.getValue().addReadException(ExceptionTypes.DISCONNECTED.getException());
        }
    }

}

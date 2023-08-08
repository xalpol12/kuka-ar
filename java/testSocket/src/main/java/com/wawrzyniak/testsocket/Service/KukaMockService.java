package com.wawrzyniak.testsocket.Service;

import com.wawrzyniak.testsocket.Model.KRLVar;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Types.RobotClasses;
import com.wawrzyniak.testsocket.Model.Types.VarType;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.util.HashSet;
import java.util.Map;
import java.util.Set;

@Service
public class KukaMockService {

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
        MockRobotListBuilder listBuilder = new MockRobotListBuilder();
        for (RobotClasses robot : RobotClasses.values()){
            Set<RobotData> robots = new HashSet<>();
            for (int i = 0; i < 3; i++){
                robots.add(new RobotDataBuilder()
                        .withName(robot.name() + "_" + String.valueOf(i))
                        .withPositionShift(i, i * 1.5, i * 2)
                        .withRotationShift(i, i*1.5, i*2)
                        .build());
            }
            listBuilder.withRobots(robot.name(), robots);
        }
        return listBuilder.build();
    }

    @Scheduled(fixedDelay = 10000)
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
    }
}

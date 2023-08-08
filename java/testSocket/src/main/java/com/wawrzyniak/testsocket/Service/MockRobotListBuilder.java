package com.wawrzyniak.testsocket.Service;

import com.wawrzyniak.testsocket.Model.Records.RobotData;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

public class MockRobotListBuilder {

    private Map<String, Map<String, RobotData>> robots;

    MockRobotListBuilder(){
        robots = new HashMap<>();
    }

    public MockRobotListBuilder withRobot(String category, RobotData robot){
        if(!robots.containsKey(category)){
            robots.put(category, new HashMap<>());
        }
        robots.get(category).put(robot.name(), robot);
        return this;
    }

    public MockRobotListBuilder withRobots(String category, Set<RobotData> robots){
        if(!this.robots.containsKey(category)){
            this.robots.put(category, new HashMap<>());
        }
        for(RobotData robot : robots){
            this.robots.get(category).put(robot.name(), robot);
        }
        return this;
    }

    public Map<String, Map<String, RobotData>> build(){
        return robots;
    }

}

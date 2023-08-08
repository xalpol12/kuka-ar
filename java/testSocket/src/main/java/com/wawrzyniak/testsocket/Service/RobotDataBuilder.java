package com.wawrzyniak.testsocket.Service;

import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Records.Vector3;

public class RobotDataBuilder {
    private Vector3 positionShift;
    private Vector3 rotationShift;
    private String name;
    public RobotDataBuilder withName(String name){
        this.name = name;
        return this;
    }

    public RobotDataBuilder withPositionShift(double x, double y, double z){
        positionShift = new Vector3(x, y, z);
        return this;
    }

    public RobotDataBuilder withRotationShift(double x, double y, double z) {
        rotationShift = new Vector3(x, y, z);
        return this;
    }

    public RobotData build(){
        return new RobotData(name, positionShift, rotationShift);
    }
}

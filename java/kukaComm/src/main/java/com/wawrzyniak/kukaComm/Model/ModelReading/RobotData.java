package com.wawrzyniak.kukaComm.Model.ModelReading;

import com.wawrzyniak.kukaComm.Model.Records.Vector3;


public record RobotData(String name, Vector3 positionShift, Vector3 rotationShift) {
}

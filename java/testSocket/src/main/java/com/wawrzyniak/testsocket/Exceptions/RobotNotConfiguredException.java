package com.wawrzyniak.testsocket.Exceptions;

public class RobotNotConfiguredException extends Exception {
    public RobotNotConfiguredException() {
        super("Requested robot does not appear in database");
    }
}

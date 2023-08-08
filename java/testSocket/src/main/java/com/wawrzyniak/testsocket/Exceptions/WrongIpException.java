package com.wawrzyniak.testsocket.Exceptions;

public class WrongIpException extends Exception {
    public WrongIpException(String cannotReachDesiredIp) {
        super(cannotReachDesiredIp);
    }
}

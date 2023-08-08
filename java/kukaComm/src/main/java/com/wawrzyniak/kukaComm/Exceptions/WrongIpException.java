package com.wawrzyniak.kukaComm.Exceptions;

public class WrongIpException extends Exception {
    public WrongIpException(String cannotReachDesiredIp) {
        super(cannotReachDesiredIp);
    }
}

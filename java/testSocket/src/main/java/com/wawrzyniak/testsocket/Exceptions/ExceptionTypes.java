package com.wawrzyniak.testsocket.Exceptions;

import java.io.IOException;

public enum ExceptionTypes {
    WRONG_IP(new WrongIpException("Cannot reach desired IP")),
    WRONG_ID(new WrongIdException("Id of received message is not matching id of send message")),
    EMPTY_BASE_OR_TOOL(new EmptyBaseOrToolException("Set tool and base of robot to check position of either")),
    DISCONNECTED(new IOException("Exception thrown when connection with robot is lost."));

    private final Exception exception;

    ExceptionTypes(Exception e) {
        exception = e;
    }

    public Exception getException() {
        return exception;
    }
}

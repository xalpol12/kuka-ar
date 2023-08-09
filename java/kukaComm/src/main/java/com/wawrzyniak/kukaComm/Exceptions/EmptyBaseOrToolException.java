package com.wawrzyniak.kukaComm.Exceptions;

public class EmptyBaseOrToolException extends Exception {
    public EmptyBaseOrToolException(String whatIsEmpty) {
        super(whatIsEmpty);
    }
}

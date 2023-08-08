package com.wawrzyniak.testsocket.Exceptions;

public class EmptyBaseOrToolException extends Exception {
    public EmptyBaseOrToolException(String whatIsEmpty){
        super(whatIsEmpty);
    }
}

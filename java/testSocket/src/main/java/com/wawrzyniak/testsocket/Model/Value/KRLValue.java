package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Exceptions.EmptyBaseOrToolException;

public interface KRLValue {
    void setValueFromString(String values) throws EmptyBaseOrToolException;
    String toJsonString() throws JsonProcessingException;

    void setRandomValues();
}

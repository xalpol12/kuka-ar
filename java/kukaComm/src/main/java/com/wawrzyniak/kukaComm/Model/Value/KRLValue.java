package com.wawrzyniak.kukaComm.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.kukaComm.Exceptions.EmptyBaseOrToolException;

public interface KRLValue {
    void setValueFromString(String values) throws EmptyBaseOrToolException;
    String toJsonString() throws JsonProcessingException;
}

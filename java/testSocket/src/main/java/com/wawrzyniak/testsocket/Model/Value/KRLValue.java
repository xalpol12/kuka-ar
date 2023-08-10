package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import com.wawrzyniak.testsocket.Config.KRLValueDeserializer;
import com.wawrzyniak.testsocket.Exceptions.EmptyBaseOrToolException;
@JsonDeserialize(using = KRLValueDeserializer.class)
public interface KRLValue {
    void setValueFromString(String values) throws EmptyBaseOrToolException;
    String toJsonString() throws JsonProcessingException;

    void setRandomValues();
}

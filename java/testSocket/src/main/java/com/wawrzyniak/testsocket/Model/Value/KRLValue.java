package com.wawrzyniak.testsocket.Model.Value;

import com.fasterxml.jackson.core.JsonProcessingException;
<<<<<<< refs/remotes/origin/main
import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import com.wawrzyniak.testsocket.Config.KRLValueDeserializer;
import com.wawrzyniak.testsocket.Exceptions.EmptyBaseOrToolException;
@JsonDeserialize(using = KRLValueDeserializer.class)
=======
import com.wawrzyniak.testsocket.Exceptions.EmptyBaseOrToolException;

>>>>>>> add testSocket and kukaComm
public interface KRLValue {
    void setValueFromString(String values) throws EmptyBaseOrToolException;
    String toJsonString() throws JsonProcessingException;

    void setRandomValues();
}

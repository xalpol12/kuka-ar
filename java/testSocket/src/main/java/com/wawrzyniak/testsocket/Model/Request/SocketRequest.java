package com.wawrzyniak.testsocket.Model.Request;

import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import com.wawrzyniak.testsocket.Config.SocketRequestDeserializer;

@JsonDeserialize(using = SocketRequestDeserializer.class)
public interface SocketRequest {
    public RequestType getRequestType();
}

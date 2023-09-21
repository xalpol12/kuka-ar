package com.wawrzyniak.kukaComm.Model.SocketRequest;

import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import com.wawrzyniak.kukaComm.Config.SocketRequestDeserializer;

@JsonDeserialize(using = SocketRequestDeserializer.class)
public interface SocketRequest {
    public RequestType getRequestType();
}



package com.wawrzyniak.testsocket.Model.Request;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class UnsubscribeRequest implements SocketRequest {

    private String unsubscribeIp;
    @Override
    public RequestType getRequestType() {
        return RequestType.UNSUBSCRIBE;
    }
}

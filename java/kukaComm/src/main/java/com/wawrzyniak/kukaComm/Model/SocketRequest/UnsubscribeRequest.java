package com.wawrzyniak.kukaComm.Model.SocketRequest;

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

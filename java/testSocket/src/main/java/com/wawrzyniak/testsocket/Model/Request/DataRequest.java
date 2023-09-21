package com.wawrzyniak.testsocket.Model.Request;

import com.wawrzyniak.testsocket.Model.Types.VarType;
import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class DataRequest implements SocketRequest{
    private String host;
    private VarType var;

    @Override
    public RequestType getRequestType() {
        return RequestType.DATA;
    }
}

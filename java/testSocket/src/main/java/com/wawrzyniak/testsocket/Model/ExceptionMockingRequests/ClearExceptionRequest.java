package com.wawrzyniak.testsocket.Model.ExceptionMockingRequests;

import com.wawrzyniak.testsocket.Model.Types.VarType;
import lombok.Data;

@Data
public class ClearExceptionRequest {
    private String hostIP;
    private VarType variable;
}

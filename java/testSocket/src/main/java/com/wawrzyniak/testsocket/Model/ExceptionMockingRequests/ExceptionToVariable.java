package com.wawrzyniak.testsocket.Model.ExceptionMockingRequests;

import com.wawrzyniak.testsocket.Exceptions.ExceptionTypes;
import com.wawrzyniak.testsocket.Model.Types.VarType;
import lombok.Data;

@Data
public class ExceptionToVariable {
    private String hostIP;
    private VarType variable;
    private ExceptionTypes exception;
}

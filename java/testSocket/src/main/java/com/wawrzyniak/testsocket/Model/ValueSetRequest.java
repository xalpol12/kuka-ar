package com.wawrzyniak.testsocket.Model;

import com.wawrzyniak.testsocket.Model.Types.VarType;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class ValueSetRequest {
    private String host;
    private VarType var;
    private KRLValue value;
}

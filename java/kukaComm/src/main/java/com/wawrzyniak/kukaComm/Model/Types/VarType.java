package com.wawrzyniak.kukaComm.Model.Types;


import com.wawrzyniak.kukaComm.Model.Value.KRLFrame;
import com.wawrzyniak.kukaComm.Model.Value.KRLInt;
import com.wawrzyniak.kukaComm.Model.Value.KRLJoints;
import com.wawrzyniak.kukaComm.Model.Value.KRLValue;

public enum VarType {
    BASE("$BASE"),
    POSITION("$POS_ACT"),
    BASE_NUMBER("$ACT_BASE"),
    TOOL_NUMBER("$ACT_TOOL"),
    JOINTS("$AXIS_ACT");

    private final String value;

    VarType(String name) {
        value = name;
    }

    public ValuePair getValue() {
        KRLValue type = switch (this) {
            case BASE, POSITION: {
                yield new KRLFrame();
            }
            case BASE_NUMBER, TOOL_NUMBER: {
                yield new KRLInt();
            }
            case JOINTS: {
                yield new KRLJoints();
            }
        };
        return new ValuePair(this.value, type);
    }
}

package com.wawrzyniak.kukaComm.Model;

import com.wawrzyniak.kukaComm.Exceptions.EmptyBaseOrToolException;
import com.wawrzyniak.kukaComm.Model.Records.Vector3;
import com.wawrzyniak.kukaComm.Model.Types.VarType;
import com.wawrzyniak.kukaComm.Model.Value.KRLFrame;
import com.wawrzyniak.kukaComm.Model.Value.KRLInt;
import com.wawrzyniak.kukaComm.Model.Value.KRLJoints;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertArrayEquals;
import static org.junit.jupiter.api.Assertions.assertEquals;

class KRLVarTest {

    //byteArray generated with third party application
    private static final Byte[] request = {0, 0, 0, 8, 0, 0, 5, 36, 66, 65, 83, 69};
    private static KRLVar base;
    private static KRLVar baseNumber;
    private static KRLVar position;
    private static KRLVar joints;

    @BeforeAll
    public static void prepare(){
        base = new KRLVar(VarType.BASE);
        baseNumber = new KRLVar(VarType.BASE_NUMBER);
        position = new KRLVar(VarType.POSITION);
        joints = new KRLVar(VarType.JOINTS);
    }

    @Test
    void getReadRequest() {
        //given
        KRLVar variable = base;
        //when
        Byte[] request = variable.getReadRequest();
        //then
        assertArrayEquals(KRLVarTest.request, request);
    }

    @Test
    void setValueInt() throws EmptyBaseOrToolException {
        //given
        String input = "5";
        KRLVar number = baseNumber;
        //when
        number.setValue(input);
        KRLInt variable = (KRLInt) number.getValue();
        //then
        assertEquals(Integer.parseInt(input), variable.getValueInt());
    }

    @Test
    void setValueFrame() throws EmptyBaseOrToolException {
        //given
        String input = "{FRAME: X 383.937897, Y 485.390259, Z 341.860138, A -89.7608185, B 0.727514744, C -0.705472112}";
        KRLFrame expected = new KRLFrame();
        expected.setPosition(new Vector3(383.937897, 485.390259, 341.860138));
        expected.setRotation(new Vector3(-0.705472112, 0.727514744, -89.7608185));
        KRLVar frame = position;
        //when
        frame.setValue(input);
        KRLFrame variable = (KRLFrame) frame.getValue();
        //then
        assertEquals(expected, variable);
    }

    @Test
    void setValueJoints() throws EmptyBaseOrToolException {
        //given
        String input = "{E6AXIS: A1 52.1, A2 77.2, A3 35.1, A4 180.2, A5 88.7, A6 126.5, E1 0.0, E2 0.0, E3 0.0, E4 0.0, E5 0.0, E6 0.0}";
        KRLJoints expected = new KRLJoints();
        expected.setJ1(52.1);
        expected.setJ2(77.2);
        expected.setJ3(35.1);
        expected.setJ4(180.2);
        expected.setJ5(88.7);
        expected.setJ6(126.5);
        KRLVar joints = KRLVarTest.joints;
        //when
        joints.setValue(input);
        KRLJoints variable = (KRLJoints) joints.getValue();
        //then
        assertEquals(expected, variable);
    }
}
package com.wawrzyniak.testsocket.Model;

import com.wawrzyniak.testsocket.Model.Value.KRLFrame;
import lombok.Data;

@Data
public class MotionDescription {
    private String ipAddress;
    private KRLFrame tcpPosition;
    private Integer desiredTime;
}

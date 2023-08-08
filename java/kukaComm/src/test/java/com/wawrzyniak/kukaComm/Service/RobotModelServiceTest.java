package com.wawrzyniak.kukaComm.Service;

import com.wawrzyniak.kukaComm.Model.ModelReading.RobotData;
import com.wawrzyniak.kukaComm.Service.RobotData.RobotModelService;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.io.IOException;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
class RobotModelServiceTest {

    @Autowired
    private RobotModelService robotService;

    @Test
    void getAvailableRobots() throws IOException {
        //given
        Map<String, Map<String, RobotData>> availableRobots;
        //when
        availableRobots = robotService.getAvailableRobots();
        //then
        assertEquals(3, availableRobots.size());
    }
}
package com.wawrzyniak.kukaComm.Service;

import com.wawrzyniak.kukaComm.Service.RobotData.RobotStickerService;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.io.IOException;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
class RobotStickerServiceTest {
    @Autowired
    private RobotStickerService robotService;

    @Test
    void getAllStickersTest() throws IOException {
        //given
        Map<String, byte[]> availableRobots;
        //when
        availableRobots = robotService.getAllStickers();
        //then
        assertEquals(2, availableRobots.size());
    }
}
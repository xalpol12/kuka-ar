package com.wawrzyniak.testsocket.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import com.wawrzyniak.testsocket.Model.ValueSetRequest;
import com.wawrzyniak.testsocket.Service.ImageService;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.Map;

@RestController
@RequiredArgsConstructor
@RequestMapping("/kuka-variables/")
public class KukaCommStaticDataController {

    private static final Logger logger = LoggerFactory.getLogger(KukaCommStaticDataController.class);

    private final KukaMockService kukaService;
    private final ImageService imageService;

    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots(){
        logger.debug("Called endpoint: GET /configured");
        return kukaService.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        logger.debug("Called endpoint: GET /stickers");
        return imageService.getAllStickers();
    }

    @PostMapping("random")
    public boolean isRandomizing(@RequestBody boolean setRandomizing){
        logger.debug("Called endpoint: POST /random, request body: " + setRandomizing);
        kukaService.setRandomizing(setRandomizing);
        return kukaService.isRandomizing();
    }

    @PostMapping("set")
    public KRLValue setValue(@RequestBody ValueSetRequest request) throws JsonProcessingException {
        logger.debug("Called endpoint: POST /configured, request body: " + request);
        return kukaService.setValue(request.getHost(), request.getVar(), request.getValue());
    }
}

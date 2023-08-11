package com.wawrzyniak.testsocket.Controller;


import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import com.wawrzyniak.testsocket.Model.ValueSetRequest;
import com.wawrzyniak.testsocket.Service.ImageService;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.Map;

@RestController
@RequestMapping("/kuka-variables/")
public class KukaCommStaticDataController {

    private final KukaMockService kukaService;
    private final ImageService imageService;

    @Autowired
    KukaCommStaticDataController(KukaMockService service, ImageService imageService){
        kukaService = service;
        this.imageService = imageService;
    }

    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots(){
        return kukaService.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        return imageService.getAllStickers();
    }

    @PostMapping("random")
    public boolean isRandomizing(@RequestBody boolean setRandomizing){
        kukaService.setRandomizing(setRandomizing);
        return kukaService.isRandomizing();
    }

    @PostMapping("set")
    public KRLValue setValue(@RequestBody ValueSetRequest request) throws JsonProcessingException {
        return kukaService.setValue(request.getHost(), request.getVar(), request.getValue());
    }

}

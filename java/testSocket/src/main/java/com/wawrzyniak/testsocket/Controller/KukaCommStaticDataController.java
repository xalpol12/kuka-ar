package com.wawrzyniak.testsocket.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.testsocket.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import com.wawrzyniak.testsocket.Model.ValueSetRequest;
import com.wawrzyniak.testsocket.Service.ConfiguredRobotService;
import com.wawrzyniak.testsocket.Service.ImageService;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/kuka-variables/")
public class KukaCommStaticDataController {

    private final KukaMockService kukaService;
    private final ImageService imageService;

    private final ConfiguredRobotService robotService;

    @Autowired
    KukaCommStaticDataController(KukaMockService service, ImageService imageService, ConfiguredRobotService robotService){
        kukaService = service;
        this.imageService = imageService;
        this.robotService = robotService;
    }

    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots(){
        return kukaService.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        return imageService.getAllStickers();
    }

    @GetMapping("robots")
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {

        return robotService.getAllConfiguredRobots();
    }
    @GetMapping("robot/{ip}")
    public ConfiguredRobotDTO getRobotByIp(@PathVariable String ip) throws RobotNotConfiguredException {
        return robotService.getRobotByIp(ip);
    }

    @PostMapping("add")
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) {
        return robotService.save(robotDTO);
    }
    @PostMapping("update")
    public ConfiguredRobotDTO updateRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        return robotService.updateByIp(robotDTO);
    }

    @DeleteMapping("delete/{ip}")
    public void deleteRobot(@PathVariable String ip) {
        robotService.deleteByIp(ip);
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

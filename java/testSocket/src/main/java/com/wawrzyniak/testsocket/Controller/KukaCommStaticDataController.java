package com.wawrzyniak.testsocket.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.testsocket.Exceptions.WrongRequestException;
import com.wawrzyniak.testsocket.Model.ExceptionMockingRequests.ClearExceptionRequest;
import com.wawrzyniak.testsocket.Model.ExceptionMockingRequests.DisconnectRequest;
import com.wawrzyniak.testsocket.Model.ExceptionMockingRequests.ExceptionToVariable;
import com.wawrzyniak.testsocket.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import com.wawrzyniak.testsocket.Model.ValueSetRequest;
import com.wawrzyniak.testsocket.Service.ConfiguredRobotService;
import com.wawrzyniak.testsocket.Service.ImageService;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;

@RestController
@RequiredArgsConstructor
@RequestMapping("/kuka-variables/")
public class KukaCommStaticDataController {

    private static final Logger logger = LoggerFactory.getLogger(KukaCommStaticDataController.class);

    private final KukaMockService kukaService;
    private final ImageService imageService;
    private final ConfiguredRobotService robotService;
  
    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots() {
        logger.debug("Called endpoint: GET /configured");
        return kukaService.getAvailableRobots();
    }

    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        logger.debug("Called endpoint: GET /stickers");
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
    public boolean isRandomizing(@RequestBody boolean setRandomizing) {
        logger.debug("Called endpoint: POST /random, request body: " + setRandomizing);
        kukaService.setRandomizing(setRandomizing);
        return kukaService.isRandomizing();
    }

    @PostMapping("set")
    public KRLValue setValue(@RequestBody ValueSetRequest request) throws JsonProcessingException {
        logger.debug("Called endpoint: POST /configured, request body: " + request);
        return kukaService.setValue(request.getHost(), request.getVar(), request.getValue());
    }

    @PostMapping("exception/add")
    public void addExceptionToVariable(@RequestBody ExceptionToVariable exception) throws WrongRequestException {
        kukaService.addExceptionToVariable(exception.getHostIP(), exception.getVariable(), exception.getException());
    }

    @PostMapping("exception/clear")
    public void clearExceptionFromVariable(@RequestBody ClearExceptionRequest exception) throws WrongRequestException {
        kukaService.removeExceptionFromVariable(exception.getHostIP(), exception.getVariable());
    }

    @PostMapping("exception/disconnect")
    public void disconnectRobot(@RequestBody DisconnectRequest request) throws WrongRequestException {
        kukaService.disconnectRobot(request.getHostIP());
    }
}

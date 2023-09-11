package com.wawrzyniak.testsocket.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.wawrzyniak.testsocket.Exceptions.RobotAlredyConfiguredException;
import com.wawrzyniak.testsocket.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.testsocket.Exceptions.WrongRequestException;
import com.wawrzyniak.testsocket.Model.ExceptionMockingRequests.ClearExceptionRequest;
import com.wawrzyniak.testsocket.Model.ExceptionMockingRequests.DisconnectRequest;
import com.wawrzyniak.testsocket.Model.ExceptionMockingRequests.ExceptionToVariable;
import com.wawrzyniak.testsocket.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.testsocket.Model.MotionDescription;
import com.wawrzyniak.testsocket.Model.Records.RobotData;
import com.wawrzyniak.testsocket.Model.Value.KRLValue;
import com.wawrzyniak.testsocket.Model.ValueSetRequest;
import com.wawrzyniak.testsocket.Service.ConfiguredRobotService;
import com.wawrzyniak.testsocket.Service.ImageService;
import com.wawrzyniak.testsocket.Service.KukaMockService;
import io.swagger.v3.oas.annotations.Operation;
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


    @Operation(
            summary = "Returns all available robots",
            description = "Available robots are defined in /RobotData folder",
            tags = {"Config"})
    @GetMapping("configured")
    public Map<String, Map<String, RobotData>> getAllRobots() {
        logger.debug("Called endpoint: GET /configured");
        return kukaService.getAvailableRobots();
    }

    @Operation(
            summary = "Returns all stickers",
            description = "Stickers are located in /RobotStickers folder",
            tags = {"Stickers"})
    @GetMapping("stickers")
    public Map<String, byte[]> getAllStickers() throws IOException {
        logger.debug("Called endpoint: GET /stickers");
        return imageService.getAllStickers();
    }

    @Operation(
            summary = "Returns all saved robots details",
            description = "Returns a whole table content from database",
            tags = {"Robots"})
    @GetMapping("robots")
    public List<ConfiguredRobotDTO> getALLRobotsWithStickers() {
        logger.debug("Called endpoint: GET /robots");
        return robotService.getAllConfiguredRobots();
    }

    @Operation(
            summary = "Returns saved robot details",
            description = "Returns a single row from database",
            tags = {"Robots"})
    @GetMapping("robot/{ip}")
    public ConfiguredRobotDTO getRobotByIp(@PathVariable String ip) throws RobotNotConfiguredException {
        logger.debug("Called endpoint: GET /robot/{}", ip);
        return robotService.getRobotByIp(ip);
    }

    @Operation(
            summary = "Saves new robot to a database",
            description = "Saves new robot and returns DTO of the saved robot",
            tags = {"Robots"})
    @PostMapping("add")
    public ConfiguredRobotDTO addRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotAlredyConfiguredException {
        logger.debug("Called: POST /add with request body: {}", robotDTO);
        return robotService.save(robotDTO);
    }

    @Operation(
            summary = "Updates already saved robot details",
            description = "Updates robot details and returns DTO of the updated robot",
            tags = {"Robots"})
    @PostMapping("update")
    public ConfiguredRobotDTO updateRobot(@RequestBody ConfiguredRobotDTO robotDTO) throws RobotNotConfiguredException {
        logger.debug("Called: POST /update with request body: {}", robotDTO);
        return robotService.updateByIp(robotDTO);
    }

    @Operation(
            summary = "Deletes robot from database",
            description = "Deletes saved robot from database by ip",
            tags = {"Robots"})
    @DeleteMapping("delete/{ip}")
    public void deleteRobot(@PathVariable String ip) {
        logger.debug("Called: DELETE /delete/{}", ip);
        robotService.deleteByIp(ip);
    }

    @Operation(
            summary = "Sets testSocket to \"random\" mode",
            description = "Randomizes returned websocket values for tracked variables",
            tags = {"Testing"})
    @PostMapping("random")
    public boolean isRandomizing(@RequestBody boolean setRandomizing) {
        logger.debug("Called endpoint: POST /random, request body: {}", setRandomizing);
        kukaService.setRandomizing(setRandomizing);
        return kukaService.isRandomizing();
    }

    @Operation(
            summary = "Sets given KRLValue",
            description = "Sets given KRLValue to a custom value defined in a request body",
            tags = {"Testing"})
    @PostMapping("set")
    public KRLValue setValue(@RequestBody ValueSetRequest request) throws JsonProcessingException {
        logger.debug("Called endpoint: POST /configured, request body: {}", request);
        return kukaService.setValue(request.getHost(), request.getVar(), request.getValue());
    }

    @Operation(
            summary = "Adds new exception to transmitted messages",
            description = "Forces websocket to include exception in " +
                    "each transmitted message for the chosen variable",
            tags = {"Testing"})
    @PostMapping("exception/add")
    public void addExceptionToVariable(@RequestBody ExceptionToVariable exception) throws WrongRequestException {
        logger.debug("Called endpoint: POST /exception/add, request body: {}", exception);
        kukaService.addExceptionToVariable(exception.getHostIP(), exception.getVariable(), exception.getException());
    }

    @Operation(
            summary = "Clears exceptions from future messages",
            description = "Clears all exception from the chosen variable",
            tags = {"Testing"})
    @PostMapping("exception/clear")
    public void clearExceptionFromVariable(@RequestBody ClearExceptionRequest exception) throws WrongRequestException {
        logger.debug("Called endpoint: POST /exception/clear, request body: {}", exception);
        kukaService.removeExceptionFromVariable(exception.getHostIP(), exception.getVariable());
    }

    @Operation(
            summary = "Simulates \"disconnected\" for chosen ip",
            description = "Disconnecting chosen robot will result in " +
                    "adding IOException to all variables of chosen robot",
            tags = {"Testing"})
    @PostMapping("exception/disconnect")
    public void disconnectRobot(@RequestBody DisconnectRequest request) throws WrongRequestException {
        logger.debug("Called endpoint: POST /exception/disconnect, request body: {}", request);
        kukaService.disconnectRobot(request.getHostIP());
    }

    @PostMapping("move")
    public void addMotion(@RequestBody MotionDescription md) throws WrongRequestException {
        logger.debug("Called endpoint: POST /move, request body: " + md);
        kukaService.addMotion(md);
    }
}

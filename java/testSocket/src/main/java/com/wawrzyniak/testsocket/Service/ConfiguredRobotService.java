package com.wawrzyniak.testsocket.Service;

import com.wawrzyniak.testsocket.Exceptions.RobotAlredyConfiguredException;
import com.wawrzyniak.testsocket.Exceptions.RobotNotConfiguredException;
import com.wawrzyniak.testsocket.Model.ModelReading.ConfiguredRobotDTO;
import com.wawrzyniak.testsocket.Model.Types.RobotClasses;
import io.swagger.v3.oas.annotations.Operation;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class ConfiguredRobotService {

    private final Map<String, ConfiguredRobotDTO> robots;
    private long id;

    public ConfiguredRobotService() {
        robots = new HashMap<>();
        id = 0;
        populateRobots();
    }

    public ConfiguredRobotDTO save(ConfiguredRobotDTO robot) throws RobotAlredyConfiguredException {
        if (robots.containsKey(robot.getIpAddress())) {
            throw new RobotAlredyConfiguredException("Robot with this ip is already configured, if you want to change it's parameters update it instead of saving new one.");
        }
        robot.setId(id++);
        robots.put(robot.getIpAddress(), robot);
        return robot;
    }

    public List<ConfiguredRobotDTO> getAllConfiguredRobots() {
        List<ConfiguredRobotDTO> robotDTOList = new ArrayList<>();
        for (Map.Entry<String, ConfiguredRobotDTO> entry : robots.entrySet()) {
            robotDTOList.add(entry.getValue());
        }
        return robotDTOList;
    }

    public ConfiguredRobotDTO getRobotByIp(String ip) throws RobotNotConfiguredException {
        ConfiguredRobotDTO robot = robots.get(ip);
        if (robot == null) {
            throw new RobotNotConfiguredException();
        }
        return robot;
    }

    public ConfiguredRobotDTO updateByIp(ConfiguredRobotDTO robot) throws RobotNotConfiguredException {
        if (!robots.containsKey(robot.getIpAddress())) {
            throw new RobotNotConfiguredException();
        }
        long id = robots.get(robot.getIpAddress()).getId();
        robot.setId(id);
        robots.put(robot.getIpAddress(), robot);
        return robot;
    }

    public void deleteByIp(String ip) {
        robots.remove(ip);
    }

    private void populateRobots() {
        for (RobotClasses robot : RobotClasses.values()) {
            for (int i = 0; i < 3; i++) {
                ConfiguredRobotDTO robotDTO =  ConfiguredRobotDTO.builder()
                                                .id(id)
                                                .category(robot.name())
                                                .name(robot.name() + " " + i)
                                                .ipAddress("192.168.1.5" + id)
                                                .build();
                id++;
                robots.put(robotDTO.getIpAddress(), robotDTO);
            }
        }
    }
}

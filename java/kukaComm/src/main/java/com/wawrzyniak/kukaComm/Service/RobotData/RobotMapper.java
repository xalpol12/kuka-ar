package com.wawrzyniak.kukaComm.Service.RobotData;

import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobot;
import com.wawrzyniak.kukaComm.Model.ModelReading.ConfiguredRobotDTO;
import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class RobotMapper {
    private ModelMapper mapper = new ModelMapper();

    public ConfiguredRobot dtoToRobot(ConfiguredRobotDTO robotDTO) {
        return mapper.map(robotDTO, ConfiguredRobot.class);
    }

    public ConfiguredRobotDTO robotToDto(ConfiguredRobot robot) {
        return mapper.map(robot, ConfiguredRobotDTO.class);
    }

    public List<ConfiguredRobotDTO> robotListToDto(List<ConfiguredRobot> list) {
        return list.stream().map(this::robotToDto).collect(Collectors.toList());
    }
}

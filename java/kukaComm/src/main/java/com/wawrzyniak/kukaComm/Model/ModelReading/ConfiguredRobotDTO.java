package com.wawrzyniak.kukaComm.Model.ModelReading;

import lombok.Data;

@Data
public class ConfiguredRobotDTO {
    private Long id;
    private String category;
    private String name;
    private String ipAddress;
}

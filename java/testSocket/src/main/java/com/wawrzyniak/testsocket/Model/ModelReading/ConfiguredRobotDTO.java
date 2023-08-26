package com.wawrzyniak.testsocket.Model.ModelReading;


import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class ConfiguredRobotDTO {
    private Long id;
    private String category;
    private String name;
    private String ipAddress;
}
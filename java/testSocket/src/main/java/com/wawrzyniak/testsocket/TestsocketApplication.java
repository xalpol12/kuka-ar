package com.wawrzyniak.testsocket;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@EnableScheduling
@SpringBootApplication
public class TestsocketApplication {

	public static void main(String[] args) {
		SpringApplication.run(TestsocketApplication.class, args);
	}

}

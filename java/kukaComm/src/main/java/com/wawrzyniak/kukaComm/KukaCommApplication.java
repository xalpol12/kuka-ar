package com.wawrzyniak.kukaComm;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;
@EnableScheduling
@SpringBootApplication
public class KukaCommApplication {

	public static void main(String[] args){
		SpringApplication.run(KukaCommApplication.class, args);
	}

}

package com.wawrzyniak.testsocket.Model.Records;

import com.wawrzyniak.testsocket.Model.Value.KRLValue;

import java.util.Set;

public record ValueException(KRLValue value, Set<ExceptionMessagePair> readExceptions) {
}

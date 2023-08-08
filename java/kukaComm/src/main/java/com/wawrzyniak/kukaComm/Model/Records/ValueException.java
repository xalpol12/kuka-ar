package com.wawrzyniak.kukaComm.Model.Records;

import com.wawrzyniak.kukaComm.Model.Value.KRLValue;
import java.util.Set;

public record ValueException(KRLValue value, Set<ExceptionMessagePair> readExceptions) {
}

import { HttpErrorResponse } from '@angular/common/http';

interface ValidationErrorPayload {
  errors?: Record<string, string[] | string>;
  message?: string;
  title?: string;
  detail?: string;
}

function sanitizeServerText(text: string): string {
  const trimmed = text.trim();
  if (trimmed.length === 0) {
    return '';
  }

  const firstLine = trimmed.split(/\r?\n/)[0]?.trim() ?? '';
  if (firstLine.length === 0) {
    return '';
  }

  if (firstLine.startsWith('System.')) {
    const colonIndex = firstLine.indexOf(':');
    if (colonIndex >= 0 && colonIndex < firstLine.length - 1) {
      return firstLine.substring(colonIndex + 1).trim();
    }
  }

  const atIndex = firstLine.indexOf(' at ');
  if (atIndex > 0) {
    return firstLine.substring(0, atIndex).trim();
  }

  return firstLine;
}

export function extractApiErrorMessage(
  error: unknown,
  fallback = 'Something went wrong. Please try again.',
): string {
  if (error == null) {
    return fallback;
  }

  if (typeof error === 'string' && error.trim().length > 0) {
    return error;
  }

  if (error instanceof Error && error.message.trim().length > 0) {
    return error.message;
  }

  if (error instanceof HttpErrorResponse) {
    if (error.status === 0) {
      return 'Cannot reach the server. Please check your connection.';
    }

    if (error.status === 403) {
      return 'Access denied. You do not have permission to perform this action.';
    }

    const payload = error.error as ValidationErrorPayload | string | null;

    if (typeof payload === 'string' && payload.trim().length > 0) {
      const sanitized = sanitizeServerText(payload);
      if (sanitized.toLowerCase().includes('access denied')) {
        return 'Access denied. You do not have permission to perform this action.';
      }
      return sanitized.length > 0 ? sanitized : fallback;
    }

    if (payload && typeof payload === 'object') {
      if (payload.message && payload.message.trim().length > 0) {
        if (payload.message.toLowerCase().includes('access denied')) {
          return 'Access denied. You do not have permission to perform this action.';
        }
        return payload.message;
      }

      if (payload.detail && payload.detail.trim().length > 0) {
        if (payload.detail.toLowerCase().includes('access denied')) {
          return 'Access denied. You do not have permission to perform this action.';
        }
        return payload.detail;
      }

      if (payload.title && payload.title.trim().length > 0) {
        if (payload.title.toLowerCase().includes('access denied')) {
          return 'Access denied. You do not have permission to perform this action.';
        }
        return payload.title;
      }

      const errors = payload.errors;
      if (errors && typeof errors === 'object') {
        const parts: string[] = [];
        for (const value of Object.values(errors)) {
          if (Array.isArray(value)) {
            for (const item of value) {
              if (item && item.trim().length > 0) {
                parts.push(item);
              }
            }
          } else if (typeof value === 'string' && value.trim().length > 0) {
            parts.push(value);
          }
        }

        if (parts.length > 0) {
          return parts.join(' ');
        }
      }
    }

    if (typeof error.message === 'string' && error.message.trim().length > 0) {
      return error.message;
    }
  }

  return fallback;
}

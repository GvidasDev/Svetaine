import apiClient from "../../api/apiClient";
import { Navigate } from "react-router-dom";
import { useEffect, useState } from "react";

export default function ProtectedRoute({ children }) {
  const [isValid, setIsValid] = useState(null);

  useEffect(() => {
    const checkToken = async () => {
      const token = localStorage.getItem("token");
      if (!token) return setIsValid(false);

      try {
        await apiClient.get("/auth/verify", {
          headers: { Authorization: `Bearer ${token}` },
        });
        setIsValid(true);
      } catch {
        setIsValid(false);
      }
    };

    checkToken();
  }, []);

  if (isValid === null) return null;
  if (!isValid) return <Navigate to="/login" replace />;
  return children;
}
